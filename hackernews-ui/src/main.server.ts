import { bootstrapApplication } from '@angular/platform-browser';
import { config } from './app/app.config.server';
import { StoryListComponent } from './app/components/story-list/story-list.component';

const bootstrap = () => bootstrapApplication(StoryListComponent, config);

export default bootstrap;
