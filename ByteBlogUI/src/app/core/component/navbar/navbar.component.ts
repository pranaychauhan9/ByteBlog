import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../features/auth/services/auth.service';
import { User } from '../../../features/auth/models/User.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: false,
  
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {

  user? :User

  constructor(private authService : AuthService,
              private router :Router
  ) {}


  ngOnInit(): void {
      this.authService.user().subscribe({
        next: (resonse) =>{
          this.user = resonse
        }
      })  

      this.user = this.authService.getUser();
      console.log(this.user)
  }


  onLogout():void{
    this.authService.logOut();
    this.router.navigateByUrl("/")
  }

  

}
