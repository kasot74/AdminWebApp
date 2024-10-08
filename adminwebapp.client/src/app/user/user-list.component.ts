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
        console.log('加载的用户:', this.users);
      },
      error => {
        this.error = '加载用户时出错: ' + error.message;
        this.isLoading = false;
        console.error('加载用户时出错:', error);
      }
    );
  }

  deleteUser(id: number): void {
    if (confirm('确定要删除这个用户吗？')) {
      this.userService.deleteUser(id).subscribe(
        () => {
          this.users = this.users.filter(user => user.userid !== id);
          console.log('用户已删除');
        },
        error => {
          console.error('删除用户时出错:', error);
          this.error = '删除用户时出错: ' + error.message;
        }
      );
    }
  }
}