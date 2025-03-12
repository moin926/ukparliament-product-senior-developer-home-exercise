import { Component, OnInit, OnChanges, SimpleChanges, Input } from '@angular/core';
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
  personForm!: FormGroup;
  departments: Department[] = [];

  constructor(
    private fb: FormBuilder,
    private departmentService: DepartmentService,
    private personService: PersonService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
    // Load departments from the API.
    this.departmentService.getDepartments().subscribe(data => {
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

/*   ngOnChanges(changes: SimpleChanges): void {
    if (changes.person && changes.person.currentValue) {
      // Patch the form with the selected person's data.
      this.personForm.patchValue(changes.person.currentValue);
    }
  } */

  initializeForm(): void {
    this.personForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', [Validators.required, this.dateInPastValidator]],
      departmentId: [null, Validators.required]
    });
  }

  // Custom validator to ensure the date is in the past.
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
      
      // Determine whether to create or update.
      if (this.person && this.person.id) {
        // Update an existing person.
        personData.id = this.person.id;
        this.personService.updatePerson(personData).subscribe({
          next: () => {
            console.log('Person updated successfully.');
          },
          error: (err) => console.error('Error updating person:', err)
        });
      } else {
        // Create a new person.
        this.personService.createPerson(personData).subscribe({
          next: (newPerson) => {
            console.log('Person created successfully:', newPerson);
          },
          error: (err) => console.error('Error creating person:', err)
        });
      }
    } else {
      console.log('Form is invalid');
      // Mark all controls as touched to display errors.
      this.personForm.markAllAsTouched();
    }
  }
}
