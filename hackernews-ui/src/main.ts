import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { StoryListComponent } from './app/components/story-list/story-list.component';

bootstrapApplication(StoryListComponent, appConfig)
  .catch((err) => console.error(err));
