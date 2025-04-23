import { Component, OnInit, signal, computed } from '@angular/core';
import { Story } from '../../models/story';
import { StoryService } from '../../services/story.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-story-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './story-list.component.html',
  styleUrls: ['./story-list.component.css']
})
export class StoryListComponent implements OnInit {
  private allStories = signal<Story[]>([]);
  searchInput = signal('');
  activePage = signal(1);
  readonly itemsPerPage = 20;
  isLoading: boolean = false;

  constructor(private storyService: StoryService) {}

  ngOnInit(): void {
    this.isLoading = true;
    this.storyService.getStories().subscribe((stories) => {
      this.allStories.set(stories);
    });
    this.isLoading = false;
  }

  // Filter stories based on the current search input
  readonly matchingStories = computed(() => {
    const query = this.searchInput().toLowerCase();
    return this.allStories().filter(story =>
      story.title.toLowerCase().includes(query)
    );
  });

  // Get paginated stories from the filtered list
  readonly currentStories = computed(() => {
    const startIdx = (this.activePage() - 1) * this.itemsPerPage;
    return this.matchingStories().slice(startIdx, startIdx + this.itemsPerPage);
  });

  // Calculate total pages for pagination
  readonly maxPages = computed(() => {
    return Math.ceil(this.matchingStories().length / this.itemsPerPage);
  });

  onSearchChanged(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.searchInput.set(value);
    this.activePage.set(1); // Reset to page 1 on new search
  }

  goToNext(): void {
    const next = this.activePage() + 1;
    if (next <= this.maxPages()) {
      this.activePage.set(next);
    }
  }

  goToPrevious(): void {
    const prev = this.activePage() - 1;
    if (prev >= 1) {
      this.activePage.set(prev);
    }
  }


}
