import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class HttpOptionsInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // TODO: move to cfg
        const baseUri = 'http://localhost:50153/api/';

        let headers = req.headers
            .set('Content-Type', 'application/json')
            .set('Accept', 'application/json');

        req = req.clone({
            url: baseUri + req.url,
            headers: headers,
            withCredentials: true
        });

        return next.handle(req);
    }
}