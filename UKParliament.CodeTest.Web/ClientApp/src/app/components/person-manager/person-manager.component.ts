import { Component, OnInit } from '@angular/core';
import { Person } from '@models/person.model';
import { PersonService } from '@services/person.service';

@Component({
  selector: 'app-person-manager',
  templateUrl: './person-manager.component.html',
  styleUrls: ['./person-manager.component.scss']
})
export class PersonManagerComponent implements OnInit {
  people: Person[] = [];
  selectedPerson: Person| null = null;

  constructor(private personService: PersonService) {}

  ngOnInit(): void {
    // Load people from the API.
    this.personService.getPeople().subscribe((data: Person[]) => {
      this.people = data;
    });
  }

  onSelectPerson(person: Person): void {
    this.selectedPerson = person;
  }
}