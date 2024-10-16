import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from './user/user-list.component';
import { UserFormComponent } from './user/user-form.component';
import { BusListComponent } from './bus/bus-list.component';

const routes: Routes = [  
  { path: 'users', component: UserListComponent },
  { path: 'user/new', component: UserFormComponent },
  { path: 'user/:id/edit', component: UserFormComponent },
  { path: 'bus', component: BusListComponent },
  { path: '', redirectTo: '/', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }