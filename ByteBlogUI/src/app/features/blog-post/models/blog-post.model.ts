import { Category } from "../../category/models/category.model";

export interface BlogPost{
     id :string;
    title: string;
    shortDescription: string;
    content: string;
    featuredImageUrl: string;
    urlHandle: string;
    publishedDate: Date; // Use Date type for datetime values
    author: string;
    isVisible: boolean;
    categories : Category[]
    

}