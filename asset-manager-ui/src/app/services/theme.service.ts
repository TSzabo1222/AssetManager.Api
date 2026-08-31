import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  isDark = signal<boolean>(false);

  constructor() {
    const saved = localStorage.getItem('theme');
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    const shouldBeDark = saved ? saved === 'dark' : prefersDark;

    this.isDark.set(shouldBeDark);
    this.applyTheme(shouldBeDark);
  }

  toggle(): void {
    const newValue = !this.isDark();
    this.isDark.set(newValue);
    this.applyTheme(newValue);
    localStorage.setItem('theme', newValue ? 'dark' : 'light');
  }

  private applyTheme(isDark: boolean): void {
    document.body.classList.toggle('dark-mode', isDark);
  }
}