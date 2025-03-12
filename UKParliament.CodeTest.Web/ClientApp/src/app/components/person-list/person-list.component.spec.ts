import { Component, ViewChild, NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { PersonListComponent } from './person-list.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { PersonService } from '@services/person.service';
import { of } from 'rxjs';
import { By } from '@angular/platform-browser';
import { PagedResult } from '@models/paged-result.model';
import { Person } from '@models/person.model';

// A simple host component to wrap the standalone PersonListComponent.
@Component({
  template: `<app-person-list></app-person-list>`
})
class TestHostComponent {
  @ViewChild(PersonListComponent) personListComponent!: PersonListComponent;
}

describe('PersonListComponent (Standalone) via TestHostComponent', () => {
  let hostFixture: ComponentFixture<TestHostComponent>;
  let hostComponent: TestHostComponent;
  let personServiceSpy: jasmine.SpyObj<PersonService>;

  // Our mock paged result: total 13 persons, first page returns 4 items.
  const mockPagedResult: PagedResult<Person> = {
    pages: 4, // Math.ceil(13 / 4) === 4
    values: [
      { id: 1, firstName: 'John', lastName: 'Smith', dateOfBirth: new Date('1991-12-01T00:00:00.000Z'), departmentId: 1 },
      { id: 2, firstName: 'Tommy', lastName: 'Townsend', dateOfBirth: new Date('1996-03-27T00:00:00.000Z'), departmentId: 2 },
      { id: 3, firstName: 'Shawn', lastName: 'Oliver', dateOfBirth: new Date('2004-12-09T00:00:00.000Z'), departmentId: 2 },
      { id: 4, firstName: 'Stephen', lastName: "O'Niel", dateOfBirth: new Date('1987-04-20T00:00:00.000Z'), departmentId: 2 }
    ]
  };

  beforeEach(async () => {
    // Force window.innerHeight to 320 so that if updatePageSize is used, it would normally calculate:
    // availableHeight = 320 * 0.8 = 256, floor(256/60) = 4.
    spyOnProperty(window, 'innerHeight', 'get').and.returnValue(320);

    // Create a spy for PersonService.
    const spy = jasmine.createSpyObj('PersonService', ['getPeoplePaged', 'deletePerson', 'getPeopleCount']);
    // Set up spy return values.
    spy.getPeopleCount.and.returnValue(of(13));
    spy.getPeoplePaged.and.returnValue(of(mockPagedResult));

    await TestBed.configureTestingModule({
      declarations: [ TestHostComponent ],
      imports: [ PersonListComponent, HttpClientTestingModule ],
      providers: [{ provide: PersonService, useValue: spy }],
      schemas: [NO_ERRORS_SCHEMA]
    })
    // Override PersonListComponent's providers to ensure our spy is used.
    .overrideComponent(PersonListComponent, {
      set: { providers: [{ provide: PersonService, useValue: spy }] }
    })
    .compileComponents();

    personServiceSpy = TestBed.inject(PersonService) as jasmine.SpyObj<PersonService>;
  });

  beforeEach(() => {
    hostFixture = TestBed.createComponent(TestHostComponent);
    hostComponent = hostFixture.componentInstance;
    hostFixture.detectChanges();

    // Access the PersonListComponent instance from the host.
    const personList = hostComponent.personListComponent;

    // Force pageSize to 4.
    personList.pageSize = 4;
    // (Optionally, if updatePageSize is called in ngOnInit, you can override it.)
    spyOn(personList, 'updatePageSize').and.callFake(function(this: PersonListComponent) {
      this.pageSize = 4;
    });

    // To ensure delete buttons are rendered, manually assign pagedPersons.
    personList.pagedPersons = mockPagedResult.values;

    // Manually call ngOnInit so that any load logic runs.
    personList.ngOnInit();
    hostFixture.detectChanges();
  });

  it('should correctly calculate total pages based on count and pageSize', () => {
    const personList = hostComponent.personListComponent;
    personList.totalCount = 13;
    personList.pageSize = 4;
    personList.calculateTotalPages();
    expect(personList.totalPages).toBe(4);
  });

  it('should disable previous button when on the first page', () => {
    const personList = hostComponent.personListComponent;
    personList.currentPage = 1;
    hostFixture.detectChanges();
    const prevButton = hostFixture.debugElement.query(By.css('.prev-button'));
    expect(prevButton).toBeTruthy('Previous button not found');
    expect(prevButton.properties['disabled']).toBeTrue();
  });

  it('should disable next button when on the last page', () => {
    const personList = hostComponent.personListComponent;
    personList.currentPage = personList.totalPages; // 4
    hostFixture.detectChanges();
    const nextButton = hostFixture.debugElement.query(By.css('.next-button'));
    expect(nextButton).toBeTruthy('Next button not found');
    expect(nextButton.properties['disabled']).toBeTrue();
  });

  it('should emit newPerson event when the new-person button is clicked', () => {
    const personList = hostComponent.personListComponent;
    spyOn(personList.newPerson, 'emit');
    const newButton = hostFixture.debugElement.query(By.css('.new-person-button'));
    expect(newButton).toBeTruthy('New person button not found');
    newButton.triggerEventHandler('click', null);
    expect(personList.newPerson.emit).toHaveBeenCalled();
  });

  it('should call deletePerson on the service when a delete button is clicked', fakeAsync(() => {
    const personList = hostComponent.personListComponent;
    personServiceSpy.deletePerson.and.returnValue(of({}));
    spyOn(personList, 'deletePerson').and.callThrough();
    const deleteButtons = hostFixture.debugElement.queryAll(By.css('.delete-button'));
    expect(deleteButtons.length).toBeGreaterThan(0, 'No delete button found');
    // Trigger click on the first delete button.
    deleteButtons[0].triggerEventHandler('click', { stopPropagation: () => {} });
    tick();
    expect(personList.deletePerson).toHaveBeenCalled();
    expect(personServiceSpy.deletePerson).toHaveBeenCalledWith(1);
  }));

  it('should navigate to next page', () => {
    const personList = hostComponent.personListComponent;
    personList.currentPage = 1;
    personList.nextPage();
    expect(personList.currentPage).toBe(2);
    expect(personServiceSpy.getPeoplePaged).toHaveBeenCalledWith(2, 4);
  });

  it('should navigate to previous page', () => {
    const personList = hostComponent.personListComponent;
    personList.currentPage = 2;
    personList.prevPage();
    expect(personList.currentPage).toBe(1);
    expect(personServiceSpy.getPeoplePaged).toHaveBeenCalledWith(1, 4);
  });
});
