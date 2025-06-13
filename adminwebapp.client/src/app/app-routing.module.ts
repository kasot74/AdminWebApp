import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserListComponent } from './user/user-list.component';
import { UserFormComponent } from './user/user-form.component';
import { BusListComponent } from './bus/bus-list.component';
import { EquationListComponent } from './equation/equation-list.component';
import { EquationFormComponent } from './equation/equation-form.component';
import { AuthGuard } from './auth/auth.guard'; 


const routes: Routes = [    
  { path: 'users', component: UserListComponent, canActivate: [AuthGuard] },
  { path: 'user/new', component: UserFormComponent, canActivate: [AuthGuard] },
  { path: 'user/:id/edit', component: UserFormComponent, canActivate: [AuthGuard] },
  { path: 'bus', component: BusListComponent },
  { path: 'equation', component: EquationListComponent },
  { path: 'equation/new', component: EquationFormComponent },
  { path: '', redirectTo: '/', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
