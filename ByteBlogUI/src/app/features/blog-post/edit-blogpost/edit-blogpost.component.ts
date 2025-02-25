import { Component, OnDestroy, OnInit } from '@angular/core';
import { BlogpostsService } from '../services/blogposts.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { BlogPost } from '../models/blog-post.model';
import { Category } from '../../category/models/category.model';
import { CategoryService } from '../../category/services/category.service';
import { UpdateRequestBlogPost } from '../models/update-blogpost-request.model';
import { ImageSelectorService } from '../../../shared/components/image-selector/image-selector.service';

@Component({
  selector: 'app-edit-blogpost',
  standalone: false,

  
  templateUrl: './edit-blogpost.component.html',
  styleUrl: './edit-blogpost.component.css'
})
export class EditBlogpostComponent implements OnInit, OnDestroy {

    id: string| null = null;
    model?: BlogPost;
    categories$! : Observable<Category[]>;
    isImageSelectorVisible : boolean =false

    selectedCategories? : string[];
    routerSubscriptions? : Subscription;
    updateBlogPostSubscription? :Subscription;
    isImageSelectedSubscription? : Subscription
    getBlogPostSubscription? :Subscription;

  constructor(private blogpostService : BlogpostsService,
              private route :ActivatedRoute,
              private categoryService:CategoryService,
              private router:Router,
              private imageService: ImageSelectorService) { }
  
  

  ngOnInit(): void {

    this.categories$ = this.categoryService.getAllCategory();
    this.routerSubscriptions =this.route.paramMap.subscribe({
      next: (params) => {
        this.id = params.get('id');

        if(this.id){
         this.getBlogPostSubscription= this.blogpostService.getBlogPostById(this.id!).subscribe({

            next: (response)=>{
              this.model =  response;
              this.selectedCategories = response.categories.map(x => x.id.toString());
            }
          })
        }
        this.isImageSelectedSubscription = this.imageService.onSelectImage()
        .subscribe({
          next: (response) => {
            if(this.model){
              this.model.featuredImageUrl = response.url;
              this.isImageSelectorVisible =false;
            }
          }
        })
       
    }})
    }
    OnFormSubmit(): void {

      if(this.model && this.id ){
        console.log("helloe" +this.selectedCategories);


        var updatedBlogpost : UpdateRequestBlogPost= {
          title: this.model.title,
          content: this.model.content,
          featuredImageUrl:this.model.featuredImageUrl,
          urlHandle :this.model.urlHandle,
          author: this.model.author,
          shortDescription:this.model.shortDescription,
          isVisible: this.model.isVisible,
          publishedDate : this.model.publishedDate,
          categories: this.selectedCategories ?? [],
          

      }
     this.updateBlogPostSubscription= this.blogpostService.updateBlogPost(this.id, updatedBlogpost)
      .subscribe({
        next: (response) => {
          this.router.navigateByUrl('admin/blogposts');
        }
      })
    }
  }

  onDelete() :void{
        if(this.id){
          this.blogpostService.deleteBlogPost( this.id)
          .subscribe({
            next: (response) => {
              this.router.navigateByUrl('admin/blogposts');
            }
          })
        }
  }
  openUrl(): void{
    if(this.model?.urlHandle){
      window.open(this.model.urlHandle,'_blank');
    }
  }

  openImageSelector():void{
    this.isImageSelectorVisible = true;

  }
  closeImageSelector(): void{
    this.isImageSelectorVisible = false;
  }



  ngOnDestroy(): void {
    this.routerSubscriptions?.unsubscribe();
    this.getBlogPostSubscription?.unsubscribe();
    this.updateBlogPostSubscription?.unsubscribe();
    this.isImageSelectedSubscription?.unsubscribe();

  }
    

}
