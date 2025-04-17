import { AfterViewInit, Component, ElementRef, QueryList, ViewChildren } from '@angular/core';

@Component({
  selector: 'app-about-page',
  imports: [],
  templateUrl: './about-page.component.html',
  styleUrl: './about-page.component.scss'
})
export class AboutPageComponent implements AfterViewInit {
  @ViewChildren('image') images!: QueryList<ElementRef<HTMLImageElement>>;

  private loadedImages: number = 0;

  allImagesLoaded: boolean = false;

  ngAfterViewInit() {
    this.images.forEach((img) => img.nativeElement.complete ? this.onImageLoad() : null);
  }

  onImageLoad() {
    this.loadedImages++;
    if (this.loadedImages === this.images.length)
      this.allImagesLoaded = true;
  }
}
