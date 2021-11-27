import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, of, ReplaySubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiUrl;

  private currentUserSource = new ReplaySubject<IUser>(1);

  currentUser$ = this.currentUserSource.asObservable();

  constructor(private httpService: HttpClient,private router: Router) { }


  loadCurrentUser(token:string){

    if(token === null){
      this.currentUserSource.next(null);
      return of(null);
    }
    let headers = new HttpHeaders();
    headers = headers.set('Authorization',`Bearer ${token}`);


    return this.httpService.get(this.baseUrl + 'account',{headers}).pipe(
      tap((user: IUser) => {
        if(user){
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
        }
      })
    )

  }


  login(values: any){
    return this.httpService.post(this.baseUrl + 'account/login', values).pipe(
      tap((user:IUser) => {
        if(user){
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
      })
    );
  }

  register(values: any){
    return this.httpService.post(this.baseUrl + 'account/register',values).pipe(
      tap((user:IUser) => {
        if(user){
          localStorage.setItem('token',user.token);
          this.currentUserSource.next(user);
        }
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/home');
  }

  checkEmailExists(email:string){
    return this.httpService.get(this.baseUrl + 'account/emailexists?email='+email);
  }



}
