import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserListComponent } from './user/user-list.component';
import { UserFormComponent } from './user/user-form.component';
import { BusListComponent } from './bus/bus-list.component';
import { FilterByTypePipe } from './pipes/filter-by-type.pipe';
@NgModule({
  declarations: [
    AppComponent,    
    UserListComponent,
    UserFormComponent,
    BusListComponent,
    FilterByTypePipe
  ],
  imports: [
    BrowserModule, 
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }