import { Component, OnInit } from '@angular/core';
import { BlogPost } from '../models/blog-post.model';
import { BlogpostsService } from '../services/blogposts.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-blog-list',
  standalone: false,
  
  templateUrl: './blog-list.component.html',
  styleUrl: './blog-list.component.css'
})
export class BlogListComponent implements OnInit{

  blogPosts$? : Observable<BlogPost[]>
  constructor(private blogService: BlogpostsService,) 
  { 

  }


  ngOnInit(): void {
      this.blogPosts$ = this.blogService.getAllBlogPosts();
      
  }


}
