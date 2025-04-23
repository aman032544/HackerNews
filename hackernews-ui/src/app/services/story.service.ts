import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environment';
import { Observable } from 'rxjs';
import { Story } from '../models/story';

@Injectable({
  providedIn: 'root'
})
export class StoryService {

  constructor(private http: HttpClient) {}
  getStories(): Observable<Story[]> {
    return this.http.get<Story[]>(environment.apiUrl+'Story');
  }
}
