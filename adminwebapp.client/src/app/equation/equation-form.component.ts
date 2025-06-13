import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EquationService } from './equation.service';
import { Equation } from './equation.model';

@Component({
  selector: 'app-equation-form',
  templateUrl: './equation-form.component.html',
  styleUrls: ['./equation-form.component.css']
})
export class EquationFormComponent implements OnInit {
  equationForm: FormGroup;
  isEditMode = false;
  Id: string  ="";

  constructor(
    private fb: FormBuilder,
    private equationService: EquationService,
    private route: ActivatedRoute,
    public router: Router
  ) {
    this.equationForm = this.fb.group({
      name: ['', Validators.required],
      equation: ['', [Validators.required]],      
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id'] && params['id'] !== 'new') {
        this.isEditMode = true;
        this.Id =  params['id'];
        this.loadUser(this.Id);
        this.equationForm.get('name')?.disable();
      }
    });
  }

  loadUser(id: string): void {
    this.equationService.getData().subscribe(
      //equation => this.Form.patchValue(equation),
      //error => console.error('載入使用者時出錯:', error)
    );
  }

  onSubmit(): void {
    if (this.equationForm.valid) {
      const equation: Equation = this.isEditMode ? 
        { ...this.equationForm.value, name: this.equationForm.get('name')?.value } : 
        this.equationForm.value;
  
      if (this.isEditMode && this.Id) {
        equation.id = this.Id;
        /*
        this.equationService.update(user).subscribe(
          () => {
            console.log('使用者已更新');
            this.router.navigate(['/equation']);
          },
          error => console.error('更新使用者時出錯:', error)
        );
        */
      } else {
        this.equationService.create(equation).subscribe(
          () => {            
            this.router.navigate(['/equation']);
          },
          error => console.error('新增時出錯:', error)
        );
      }
    }
  }
}
