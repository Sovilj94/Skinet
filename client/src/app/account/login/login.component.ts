import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  returnUrl:string;
  loginForm: FormGroup;

  constructor(private accountService: AccountService,private router:Router,private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.createLoginForm();
    this.returnUrl = this.activatedRoute.snapshot.queryParams.returnUrl || '/shop';
  }

  createLoginForm(){
    this.loginForm = new FormGroup ({
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('',[Validators.required])
    })
  }

  onSubmit(){

    this.accountService.login(this.loginForm.value).subscribe(user => {
        this.router.navigateByUrl(this.returnUrl);
    }, error => console.log(error))

  }

  snap(){
    console.log(this.activatedRoute.snapshot);
  }

}
