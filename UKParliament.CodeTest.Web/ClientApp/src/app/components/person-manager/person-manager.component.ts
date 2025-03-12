import { Component, ViewChild } from '@angular/core';
import { Person } from '@models/person.model';
import { PersonListComponent } from '../person-list/person-list.component';

@Component({
  selector: 'app-person-manager',
  templateUrl: './person-manager.component.html',
  styleUrls: ['./person-manager.component.scss']
})
export class PersonManagerComponent {
  selectedPerson: Person | null = null;
  
  @ViewChild(PersonListComponent)
  private personListComponent!: PersonListComponent;

  onSelectPerson(person: Person): void {
    this.selectedPerson = person;
  }

  onPersonSaved(newPerson: Person): void {
    // Refresh the person list to include the new/updated record.
    if (this.personListComponent) {
      this.personListComponent.loadPagedPersons();
    }
    // Optionally update the selected person.
    this.selectedPerson = newPerson;
  }

  onNewPerson(): void {
    // Set selectedPerson to a new blank record.
    this.selectedPerson = { 
      id: null,
      firstName: '',
      lastName: '', 
      dateOfBirth: null, 
      departmentId: 0 
    } as unknown as Person;
  }
}
