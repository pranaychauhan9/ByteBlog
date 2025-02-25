import { Component, OnDestroy } from '@angular/core';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { CategoryListComponent } from '../category-list/category-list.component';
import { CategoryService } from '../services/category.service';
import { Subscription } from 'rxjs';
import {  Router } from '@angular/router';

@Component({
  selector: 'app-add-category',
  standalone: false,
  
  templateUrl: './add-category.component.html',
  styleUrl: './add-category.component.css'
})
export class AddCategoryComponent implements OnDestroy {
  model :AddCategoryRequest;

  addCategrorySubscription? : Subscription

  constructor(private categoryService : CategoryService,private router:Router){
    this.model ={
      name: '',
      urlHandle:''
    };
  }
 

  onFormSubmit(){
    console.log("Hello");
    this.categoryService.addCategory(this.model)
    .subscribe({
      next: (response) => {
        this.router.navigateByUrl('/admin/categories');
      }
    }
      
    );
  }

  ngOnDestroy(): void {
   this.addCategrorySubscription?.unsubscribe();
  }

}
