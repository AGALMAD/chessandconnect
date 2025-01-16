import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpParams,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, lastValueFrom } from 'rxjs';
import { Result } from '../models/result';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private BASE_URL = environment.apiUrl+"api/";
  jwt: string = '';

  constructor(private http: HttpClient) { }

  async get<T = void>(
    path: string,
    params: any = {},
    responseType = null
  ): Promise<Result<T>> {
    const url = `${this.BASE_URL}${path}`;
    const request$ = this.http.get(url, {
      params: new HttpParams({ fromObject: params }),
      headers: this.getHeader(),
      responseType: responseType,
      observe: 'response',
    });

    return this.sendRequest<T>(request$);
  }

  async post<T = void>(
    path: string,
    body: Object = {},
    // contentType = null
    contentType: string | null = 'application/json'
  ): Promise<Result<T>> {
    const url = `${this.BASE_URL}${path}`;
    const request$ = this.http.post(url, body, {
      headers: this.getHeader(contentType),
      observe: 'response',
    });

    return this.sendRequest<T>(request$);
  }
  async postPolling<T = void>(
    path: string,
    body: any = {},
    // contentType = null
    contentType: string | null = 'application/json'
  ) {
    const url = `${this.BASE_URL}${path}`;
    const request$ = this.http
      .post(url, body, {
        headers: this.getHeader(contentType),
        observe: 'response',
      })
      .subscribe({
        next: () => console.log('Temporary order refreshed'),
        error: (err) => console.error('Error refreshing temporary order', err),
      });
  }

  async put<T = void>(
    path: string,
    body: Object = {},
    contentType: string | null = 'application/json'
  ): Promise<Result<T>> {
    const url = `${this.BASE_URL}${path}`;
    const request$ = this.http.put(url, body, {
      headers: this.getHeader(contentType),
      observe: 'response',
    });

    return this.sendRequest<T>(request$);
  }

  async delete<T = void>(
    path: string,
    params: any = {},
    contentType: string | null = 'application/json'
  ): Promise<Result<T>> {
    const url = `${this.BASE_URL}${path}`;
    const request$ = this.http.delete(url, {
      params: new HttpParams({ fromObject: params }),
      headers: this.getHeader(contentType),
      observe: 'response',
    });

    return this.sendRequest<T>(request$);
  }

  private async sendRequest<T = void>(
    request$: Observable<HttpResponse<any>>
  ): Promise<Result<T>> {
    try {
      const response = await lastValueFrom(request$);
      const statusCode = response.status;

      if (response.ok) {
        const data = response.body as T;
        return data === undefined
          ? Result.success(statusCode)
          : Result.success(statusCode, data);
      } else {
        return Result.error(
          statusCode,
          response.statusText || 'Unexpected error'
        );
      }
    } catch (exception) {
      if (exception instanceof HttpErrorResponse) {
        return Result.error(
          exception.status,
          exception.message || exception.statusText
        );
      } else {
        return Result.error(-1, exception.message || 'Unknown error');
      }
    }
  }

  private getHeader(accept = null, contentType = null): HttpHeaders {
    let header: any = {};
    const token = this.jwt || localStorage.getItem('token');
    // Para cuando haya que poner un JWT
    header['Authorization'] = `Bearer ${token}`;

    if (accept) header['Accept'] = accept;

    if (contentType) header['Content-Type'] = contentType;

    return new HttpHeaders(header);
  }
}