import { Component, Input, Output, EventEmitter } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PersonManagerComponent } from './person-manager.component';
import { Person } from '@models/person.model';
import { By } from '@angular/platform-browser';

// Stub for the PersonListComponent
@Component({
  selector: 'app-person-list',
  template: ''
})
class PersonListStubComponent {
  @Output() personSelected = new EventEmitter<Person>();
  @Output() newPerson = new EventEmitter<void>();
}

// Stub for the PersonEditorComponent
@Component({
  selector: 'app-person-editor',
  template: ''
})
class PersonEditorStubComponent {
  @Input() person: Person | null = null;
}

describe('PersonManagerComponent', () => {
  let component: PersonManagerComponent;
  let fixture: ComponentFixture<PersonManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        PersonManagerComponent,
        PersonListStubComponent,
        PersonEditorStubComponent
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the PersonManagerComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should update selectedPerson when personSelected is emitted from PersonList', () => {
    const testPerson: Person = { id: 1, firstName: 'John', lastName: 'Doe', dateOfBirth: new Date(), departmentId: 1 };
    // Find the PersonList stub.
    const personListDebugEl = fixture.debugElement.query(By.directive(PersonListStubComponent));
    const personListStub = personListDebugEl.componentInstance as PersonListStubComponent;
    // Emit the event.
    personListStub.personSelected.emit(testPerson);
    fixture.detectChanges();
    expect(component.selectedPerson).toEqual(testPerson);
  });

  it('should set selectedPerson to a new blank record when newPerson is emitted from PersonList', () => {
    // Find the PersonList stub.
    const personListDebugEl = fixture.debugElement.query(By.directive(PersonListStubComponent));
    const personListStub = personListDebugEl.componentInstance as PersonListStubComponent;
    // Emit the newPerson event.
    personListStub.newPerson.emit();
    fixture.detectChanges();
    // Adjust the expected blank object to match your component's logic.
    const expectedBlank: Person = { 
      id: null, 
      firstName: '', 
      lastName: '', 
      dateOfBirth: null, 
      departmentId: 0 
    } as unknown as Person;
    expect(component.selectedPerson).toEqual(expectedBlank);
  });
});
