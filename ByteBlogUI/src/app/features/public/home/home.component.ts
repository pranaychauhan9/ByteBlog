import { Component, OnInit } from '@angular/core';
import { BlogpostsService } from '../../blog-post/services/blogposts.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../../blog-post/models/blog-post.model';

@Component({
  selector: 'app-home',
  standalone: false,
  
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  blogs$? : Observable<BlogPost[]>

  constructor(private blogservice: BlogpostsService){

  }
  ngOnInit(): void {
   this.blogs$ = this.blogservice.getAllBlogPosts();
  }

}
