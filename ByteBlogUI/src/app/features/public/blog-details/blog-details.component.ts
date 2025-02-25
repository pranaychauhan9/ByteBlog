import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogpostsService } from '../../blog-post/services/blogposts.service';
import { Observable } from 'rxjs';
import { BlogPost } from '../../blog-post/models/blog-post.model';

@Component({
  selector: 'app-blog-details',
  standalone: false,
  
  templateUrl: './blog-details.component.html',
  styleUrl: './blog-details.component.css'
})
export class BlogDetailsComponent implements OnInit{

  url : string | null = null
  blogPost$? : Observable<BlogPost>

  constructor(private rouete : ActivatedRoute,
              private blogPostService: BlogpostsService){

  }
  ngOnInit(): void {

    this.rouete.paramMap
    .subscribe({
      next: (params) => {
       this.url= params.get('url');
      }
    })
        

    if(this.url){
      this.blogPost$ = this.blogPostService.getBlogPostByUrlHandle(this.url)
    }
  }



}
