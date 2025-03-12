import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from '@components/home/home.component';
import { PersonManagerComponent } from '@components/person-manager/person-manager.component';
import { PersonListComponent } from './components/person-list/person-list.component';
import { PersonEditorComponent } from './components/person-editor/person-editor.component';

@NgModule({ 
    declarations: [
        AppComponent,
    PersonManagerComponent,
    PersonEditorComponent,
        HomeComponent
    ],
    bootstrap: [AppComponent], 
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' }
        ]),
        BrowserModule,
        HttpClientModule,
        ReactiveFormsModule,
        PersonListComponent,
    ], 
    providers: [provideHttpClient(withInterceptorsFromDi())] 
})
export class AppModule { }
