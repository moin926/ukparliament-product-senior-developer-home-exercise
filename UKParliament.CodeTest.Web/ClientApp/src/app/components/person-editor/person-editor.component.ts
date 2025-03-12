import { Component, OnInit, OnChanges, SimpleChanges, Input, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl } from '@angular/forms';
import { Person } from '@models/person.model';
import { Department } from '@models/department.model';
import { DepartmentService } from '@services/department.service';
import { PersonService } from '@services/person.service';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnInit, OnChanges {
  @Input() person: Person | null = null;
  @Output() personSaved = new EventEmitter<Person>();
  personForm!: FormGroup;
  departments: Department[] = [];

  constructor(
    private fb: FormBuilder,
    private departmentService: DepartmentService,
    private personService: PersonService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    // Load departments (cached by DepartmentService)
    this.departmentService.getDepartments().subscribe((data: Department[]) => {
      this.departments = data;
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.person && changes.person.currentValue) {
      // Create a shallow copy of the person
      const personValue = { ...changes.person.currentValue };
      if (personValue.dateOfBirth) {
        // Format the date to yyyy-MM-dd
        personValue.dateOfBirth = new Date(personValue.dateOfBirth)
          .toISOString()
          .substring(0, 10);
      }
      this.personForm.patchValue(personValue);
    }
  }

  initializeForm(): void {
    this.personForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', [Validators.required, this.dateInPastValidator]],
      departmentId: [null, Validators.required]
    });
  }

  // Custom validator: Ensure the date is in the past.
  dateInPastValidator(control: AbstractControl) {
    const dateValue = new Date(control.value);
    if (!control.value || dateValue >= new Date()) {
      return { dateInvalid: 'Date of birth must be in the past' };
    }
    return null;
  }

  onSave(): void {
    if (this.personForm.valid) {
      const personData: Person = this.personForm.value;
      
      // Check if we're updating an existing person or creating a new one.
      if (this.person && this.person.id) {
        // Update existing person.
        personData.id = this.person.id;
        this.personService.updatePerson(personData).subscribe({
          next: () => {
            console.log('Person updated successfully.');
            this.personSaved.emit(personData);
          },
          error: (err: any) => console.error('Error updating person:', err)
        });
      } else {
        // Create new person.
        this.personService.createPerson(personData).subscribe({
          next: (newPerson: Person) => {
            console.log('Person created successfully:', newPerson);
            this.personSaved.emit(newPerson);
          },
          error: (err: any) => console.error('Error creating person:', err)
        });
      }
    } else {
      console.log('Form is invalid');
      this.personForm.markAllAsTouched();
    }
  }
}
