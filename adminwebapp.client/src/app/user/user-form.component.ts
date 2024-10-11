import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from './user.service';
import { User } from './user.model';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  isEditMode = false;
  userId: number | null = null;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute,
    public router: Router
  ) {
    this.userForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],      
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id'] && params['id'] !== 'new') {
        this.isEditMode = true;
        this.userId = +params['id'];
        this.loadUser(this.userId);
        this.userForm.get('username')?.disable();
      }
    });
  }

  loadUser(id: number): void {
    this.userService.getUser(id).subscribe(
      user => this.userForm.patchValue(user),
      error => console.error('載入使用者時出錯:', error)
    );
  }

  onSubmit(): void {
    if (this.userForm.valid) {
      const user: User = this.isEditMode ? 
        { ...this.userForm.value, username: this.userForm.get('username')?.value } : 
        this.userForm.value;
  
      if (this.isEditMode && this.userId) {
        user.userid = this.userId;
        this.userService.updateUser(user).subscribe(
          () => {
            console.log('使用者已更新');
            this.router.navigate(['/users']);
          },
          error => console.error('更新使用者時出錯:', error)
        );
      } else {
        this.userService.createUser(user).subscribe(
          () => {
            console.log('使用者已創建');
            this.router.navigate(['/users']);
          },
          error => console.error('創建使用者時出錯:', error)
        );
      }
    }
  }
}