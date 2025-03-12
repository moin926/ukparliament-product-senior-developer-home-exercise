import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Department } from '@models/department.model';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private apiPath: string = 'api/department';
  private departments: Department[] | null = null;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  getDepartments(): Observable<Department[]> {
    if (this.departments) {
      return of(this.departments);
    }
    return this.http.get<Department[]>(`${this.baseUrl}${this.apiPath}`).pipe(
      tap(depts => this.departments = depts)
    );
  }
}
