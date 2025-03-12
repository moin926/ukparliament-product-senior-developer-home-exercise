import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Person } from '../models/person.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({               
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  private apiPath: string = 'api/person';

  // Retrieves all people (could be extended to support paging if needed)
  getPeopleCount(): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}${this.apiPath}/count`);
  }

  // Retrieves all people (could be extended to support paging if needed)
  getPeople(): Observable<Person[]> {
    return this.http.get<Person[]>(`${this.baseUrl}${this.apiPath}`);
  }

  // Retrieves a paged result of people
  getPeoplePaged(pageNumber: number = 1, pageSize: number = 10): Observable<PagedResult<Person>> {
    return this.http.get<PagedResult<Person>>(`${this.baseUrl}${this.apiPath}/paged?pageNumber=${pageNumber}&pageSize=${pageSize}`);
  }

  // Retrieves a single person by id
  getPerson(id: number): Observable<Person> {
    return this.http.get<Person>(`${this.baseUrl}${this.apiPath}/${id}`);
  }

  // Creates a new person
  createPerson(person: Person): Observable<Person> {
    return this.http.post<Person>(`${this.baseUrl}${this.apiPath}`, person);
  }

  // Updates an existing person
  updatePerson(person: Person): Observable<any> {
    return this.http.put(`${this.baseUrl}${this.apiPath}/${person.id}`, person);
  }

  // Deletes a person by id
  deletePerson(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}${this.apiPath}/${id}`);
  }
}

/*

import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonViewModel } from '../models/person-view-model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  // Below is some sample code to help get you started calling the API
  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(this.baseUrl + `api/person/${id}`)
  }
}
*/
