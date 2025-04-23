import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StoryListComponent } from './story-list.component';
import { StoryService } from '../../services/story.service';
import { of } from 'rxjs';
import { Story } from '../../models/story';
import { By } from '@angular/platform-browser';

// Sample test data
const mockStories: Story[] = Array.from({ length: 50 }, (_, i) => ({
  id: i + 1,
  title: `Story ${i + 1}`,
  url: `https://example.com/story-${i + 1}`,
}));

describe('StoryListComponent', () => {
  let component: StoryListComponent;
  let fixture: ComponentFixture<StoryListComponent>;
  let storyServiceSpy: jasmine.SpyObj<StoryService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('StoryService', ['getStories']);

    await TestBed.configureTestingModule({
      imports: [StoryListComponent],
      providers: [{ provide: StoryService, useValue: spy }],
    }).compileComponents();

    storyServiceSpy = TestBed.inject(StoryService) as jasmine.SpyObj<StoryService>;
    storyServiceSpy.getStories.and.returnValue(of(mockStories));

    fixture = TestBed.createComponent(StoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should load stories on init', () => {
    expect(component['allStories']().length).toBe(50);
  });

  it('should filter stories by search input', () => {
    component.searchInput.set('Story 1');
    fixture.detectChanges();

    const filtered = component.matchingStories();
    expect(filtered.length).toBeGreaterThan(0);
    expect(filtered.every(s => s.title.includes('Story 1'))).toBeTrue();
  });

  it('should return paginated stories based on current page', () => {
    component.activePage.set(2);
    fixture.detectChanges();

    const paginated = component.currentStories();
    expect(paginated.length).toBe(component.itemsPerPage);
    expect(paginated[0].title).toBe('Story 21');
  });

  it('should update page correctly on next and previous', () => {
    expect(component.activePage()).toBe(1);

    component.goToNext();
    expect(component.activePage()).toBe(2);

    component.goToPrevious();
    expect(component.activePage()).toBe(1);
  });

  it('should not go beyond page limits', () => {
    component.activePage.set(component.maxPages());
    component.goToNext();
    expect(component.activePage()).toBe(component.maxPages());

    component.activePage.set(1);
    component.goToPrevious();
    expect(component.activePage()).toBe(1);
  });
});
