import { Component, OnInit, HostListener, Output, EventEmitter } from '@angular/core';
import { Person } from '@models/person.model';
import { PagedResult } from '@models/paged-result.model';
import { PersonService } from '@services/person.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss'],
  imports: [CommonModule],
  standalone: true
})
export class PersonListComponent implements OnInit {
  pagedPersons: Person[] = [];
  currentPage: number = 1;
  public pageSize: number = 4; // initial default; will be updated dynamically
  totalPages: number = 1;
  totalCount: number = 0;

  @Output() personSelected = new EventEmitter<Person>();
  @Output() newPerson = new EventEmitter<void>();

  constructor(private personService: PersonService) {}

  ngOnInit(): void {
    // First, update the page size based on the viewport height.
    this.updatePageSize();
    // Load the total count, then load the persons.
    this.loadPersonCount().then(() => {
      this.loadPagedPersons();
    });
  }

  @HostListener('window:resize', ['$event'])
  onResize(): void {
    // When resizing, update page size and recalc total pages.
    this.updatePageSize();
    this.calculateTotalPages();

    // Ensure current page is still valid.
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages;
    }
    this.loadPagedPersons();
  }

  updatePageSize(): void {
    // Assume the person list container uses 80% of the viewport height.
    // Each list item is assumed to be approximately 60px tall.
    const availableHeight = window.innerHeight * 0.8;
    const itemHeight = 60;
    const newPageSize = Math.floor(availableHeight / itemHeight);

    // Ensure at least 1 item per page.
    if (newPageSize > 0 && newPageSize !== this.pageSize) {
      this.pageSize = newPageSize;
      this.calculateTotalPages();
    }
  }

  loadPersonCount(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.personService.getPeopleCount().subscribe(
        (count: number) => {
          this.totalCount = count;
          this.calculateTotalPages();
          resolve();
        },
        (error: any) => {
          console.error("Error getting person count", error);
          reject(error);
        }
      );
    });
  }

  calculateTotalPages(): void {
    if (this.totalCount && this.pageSize) {
      this.totalPages = Math.ceil(this.totalCount / this.pageSize);
    } else {
      this.totalPages = 1;
    }
  }

  loadPagedPersons(): void {
    this.personService.getPeoplePaged(this.currentPage, this.pageSize).subscribe(
      (result: PagedResult<Person>) => {
        this.pagedPersons = result.values;
        // Optionally update totalPages based on the API response if available.
        if (result.pages) {
          this.totalPages = result.pages;
        }
      },
      (error: any) => {
        console.error('Error loading paged persons', error);
      }
    );
  }

  onSelect(person: Person): void {
    this.personSelected.emit(person);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadPagedPersons();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadPagedPersons();
    }
  }

  createNewPerson(): void {
    this.newPerson.emit();
  }

  deletePerson(person: Person, event: Event): void {
    // Prevent click from selecting the person.
    event.stopPropagation();

    if (person && person.id) {
      this.personService.deletePerson(person.id).subscribe({
        next: () => {
          console.log(`Deleted person ${person.id}`);
          // Update count and reload the list.
          this.loadPersonCount().then(() => {
            if (this.currentPage > this.totalPages) {
              this.currentPage = this.totalPages;
            }
            this.loadPagedPersons();
          });
        },
        error: (err: any) => {
          console.error('Error deleting person', err);
        }
      });
    }
  }
}
