import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-account-form',
  template: `
    <form [formGroup]="accountForm" (ngSubmit)="onSubmit()">
      <div>
        <label for="username">帳號:</label>
        <input id="username" type="text" formControlName="username">
      </div>
      <div>
        <label for="password">密碼:</label>
        <input id="password" type="password" formControlName="password">
      </div>
      <div>
        <label for="confirmPassword">確認密碼:</label>
        <input id="confirmPassword" type="password" formControlName="confirmPassword">
      </div>
      <button type="submit" [disabled]="!accountForm.valid">提交</button>
    </form>
  `,
  styles: [`
    form {
      display: flex;
      flex-direction: column;
      max-width: 300px;
      margin: 0 auto;
    }
    div {
      margin-bottom: 10px;
    }
    label {
      display: block;
      margin-bottom: 5px;
    }
    input {
      width: 100%;
      padding: 5px;
    }
    button {
      padding: 10px;
      background-color: #4CAF50;
      color: white;
      border: none;
      cursor: pointer;
    }
    button:disabled {
      background-color: #cccccc;
    }
  `]
})
export class AccountFormComponent {
  accountForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.accountForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(4)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.checkPasswords });
  }

  checkPasswords(group: FormGroup) {
    const password = group.get('password')?.value;
    const confirmPassword = group.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { notSame: true };
  }

  onSubmit() {
    if (this.accountForm.valid) {
      console.log(this.accountForm.value);
      // 这里可以添加发送数据到服务器的逻辑
    }
  }
}