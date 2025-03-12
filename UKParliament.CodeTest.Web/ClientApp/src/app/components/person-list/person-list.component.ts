import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Person } from '@models/person.model';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent {
  @Input() people: Person[] = [];
  @Output() personSelected = new EventEmitter<Person>();

  onSelect(person: Person): void {
    this.personSelected.emit(person);
  }
}
