import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, retry } from 'rxjs';
import { AddCategoryRequest } from '../models/add-category-request.model';
import { Category } from '../models/category.model';
import { UpdateCategoryRequest } from '../models/update-category-request.model';
import { environment } from '../../../../environments/environment';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {


  constructor(private http: HttpClient,
              private cookieService : CookieService
  ) {

  }

  
  getAllCategory() :Observable<Category[]>{
    return this.http.get<Category[]>(`${environment.apiBaseUrl}/api/Categories`);
  }

  getCategoryById(id :string) :Observable<Category>{
    return this.http.get<Category>(`${environment.apiBaseUrl}/api/categories/${id}`)
  }


  addCategory(model :AddCategoryRequest): Observable<void>{
    return this.http.post<void>(`${environment.apiBaseUrl}/api/Categories?addAuth=true`,model);
  }

  updateCategory(id:string,updateCategory: UpdateCategoryRequest) :Observable<Category>{
    return this.http.put<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`,updateCategory,
      
    );
  }

  deleteCategory(id:string) :Observable<Category>{
    return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories/${id}?addAuth=true`)
  }

// Code below is without Inceptor Authorization

  // addCategory(model :AddCategoryRequest): Observable<void>{
  //   return this.http.post<void>(`${environment.apiBaseUrl}/api/Categories`,model,
  //     {
  //       headers: {
  //         'Authorization' : this.cookieService.get('Authorization')
  //       }
  //     }
  // );
  // }

  // updateCategory(id:string,updateCategory: UpdateCategoryRequest) :Observable<Category>{
  //   return this.http.put<Category>(`${environment.apiBaseUrl}/api/categories/${id}`,updateCategory,
  //     {
  //       headers: {
  //         'Authorization' : this.cookieService.get('Authorization')
  //       }
  //     }
      
  //   );
  // }

  // deleteCategory(id:string) :Observable<Category>{
  //   return this.http.delete<Category>(`${environment.apiBaseUrl}/api/categories/${id}`,
  //     {
  //       headers: {
  //         'Authorization' : this.cookieService.get('Authorization')
  //       }
  //     }
  //   )
  // }
}
