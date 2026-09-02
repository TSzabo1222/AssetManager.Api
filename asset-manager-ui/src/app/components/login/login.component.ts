import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  isRegisterMode = false;

  fullName = '';
  email = '';
  password = '';
  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) {}

  toggleMode(): void {
    this.isRegisterMode = !this.isRegisterMode;
    this.errorMessage = '';
  }

  submit(): void {
    this.errorMessage = '';
    this.isLoading = true;

    const request$ = this.isRegisterMode
      ? this.authService.register({ fullName: this.fullName, email: this.email, password: this.password })
      : this.authService.login({ email: this.email, password: this.password });

    request$.subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/assets']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = this.isRegisterMode
          ? (err.error ?? 'Registration failed. Please try again.')
          : (err.error ?? 'Invalid email or password.');
      }
    });
  }
}