import { Component, OnInit } from '@angular/core';
import { EquationService } from './equation.service';
import { Equation } from './equation.model';

@Component({
  selector: 'app-equation-list',
  templateUrl: './equation-list.component.html',  
  styleUrls: ['./equation-list.component.css']
})
export class EquationListComponent implements OnInit {
  equations: Equation[] = [];  
  isLoading = false;
  error: string | null = null;

  constructor(private equationService: EquationService) { }

  ngOnInit(): void {
    this.loaddata();
  }

  loaddata(): void {
    this.isLoading = true;
    this.error = null;
    this.equationService.getData().subscribe(
      (Equationinfo: Equation[]) => {
        this.equations = Equationinfo;        
        this.isLoading = false;        
      },
      error => {
        this.error = '加載使用者時出錯: ' + error.message;
        this.isLoading = false;        
      }
    );
  }
  
}
