import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { PersonEditorComponent } from './person-editor.component';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { CommonModule } from '@angular/common';
import { NgModule, NO_ERRORS_SCHEMA, SimpleChange, SimpleChanges } from '@angular/core';
import { Person } from '@models/person.model';
import { By } from '@angular/platform-browser';
import { of } from 'rxjs';

// DummyModule wraps the standalone PersonEditorComponent.
@NgModule({
  declarations: [PersonEditorComponent],
  imports: [CommonModule, ReactiveFormsModule, HttpClientTestingModule],
  exports: [PersonEditorComponent],
  providers: [
    // Provide any additional tokens (like BASE_URL) here if needed.
    { provide: 'BASE_URL', useValue: 'http://localhost:44416' }
  ]
})
class DummyModule {}

describe('PersonEditorComponent (Standalone)', () => {
  let component: PersonEditorComponent;
  let fixture: ComponentFixture<PersonEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DummyModule],
      schemas: [NO_ERRORS_SCHEMA] // Ignore unknown elements, if any.
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PersonEditorComponent);
    component = fixture.componentInstance;
    // First, call ngOnInit to create the reactive form.
    component.ngOnInit();
    // Now set the input person.
    component.person = {
      id: 1,
      firstName: 'Test',
      lastName: 'User',
      dateOfBirth: new Date('2000-01-01'),
      departmentId: 1
    } as Person;
    // Create a SimpleChanges object to simulate input change.
    const changes: SimpleChanges = {
      person: new SimpleChange(null, component.person, true)
    };
    // Call ngOnChanges with the changes.
    component.ngOnChanges(changes);
    fixture.detectChanges();
  });

  it('should create the PersonEditorComponent', () => {
    expect(component).toBeTruthy();
  });

  it('should mark the form as invalid when required fields are missing', () => {
    component.personForm.get('firstName')?.setValue('');
    fixture.detectChanges();
    expect(component.personForm.invalid).toBeTrue();
  });
});
