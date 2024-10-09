import { Component, OnInit } from '@angular/core';
import { UserService } from './user.service';
import { User } from './user.model';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html'  
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  isLoading = false;
  error: string | null = null;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.error = null;
    this.userService.getUsers().subscribe(
      (users: User[]) => {
        this.users = users;
        this.isLoading = false;
        console.log('加載的使用者:', this.users);
      },
      error => {
        this.error = '加載使用者時出錯: ' + error.message;
        this.isLoading = false;
        console.error('加載使用者時出錯:', error);
      }
    );
  }

  deleteUser(id: number): void {
    if (confirm('確定要刪除這個使用者嗎？')) {
      this.userService.deleteUser(id).subscribe(
        () => {
          this.users = this.users.filter(user => user.userid !== id);
          console.log('使用者已刪除');
        },
        error => {
          console.error('刪除使用者時出錯:', error);
          this.error = '刪除使用者時出錯: ' + error.message;
        }
      );
    }
  }
}