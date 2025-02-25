import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { AddBlogPostRequest } from '../models/add-blogpost-request.model';
import { environment } from '../../../../environments/environment';
import { UpdateRequestBlogPost } from '../models/update-blogpost-request.model';

@Injectable({
  providedIn: 'root'
})
export class BlogpostsService {

  constructor(private http : HttpClient) { 


  }


  addBlogPosts(model: AddBlogPostRequest): Observable<BlogPost>{
    return this.http.post<BlogPost>(`${environment.apiBaseUrl}/api/BlogPosts?addAuth=true`,model);
  }


  getAllBlogPosts():Observable<BlogPost[]>{
    return this.http.get<BlogPost[]>(`${environment.apiBaseUrl}/api/BlogPosts`);
  }
  getBlogPostById(id: string): Observable<BlogPost>{

    return this.http.get<BlogPost>(`${environment.apiBaseUrl}/api/BlogPosts/${id}`);
  }

  getBlogPostByUrlHandle(urlHandle: string): Observable<BlogPost>{

    return this.http.get<BlogPost>(`${environment.apiBaseUrl}/api/BlogPosts/${urlHandle}`);
  }

  updateBlogPost(id :string,updateBlogPost: UpdateRequestBlogPost):Observable<BlogPost>{
    return this.http.put<BlogPost>(`${environment.apiBaseUrl}/api/BlogPosts/${id}?addAuth=true`,updateBlogPost)
  }

  deleteBlogPost(id :string):Observable<BlogPost>{
    return this.http.delete<BlogPost>(`${environment.apiBaseUrl}/api/BlogPosts/${id}?addAuth=true`)
  }
}
