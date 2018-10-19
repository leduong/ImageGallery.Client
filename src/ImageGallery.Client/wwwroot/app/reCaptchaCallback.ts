import { Injectable } from '@angular/core'
@Injectable()
export class ReCaptchaService {
  reCaptchaCallback_Promise: Promise<any>

  constructor() {
    this.reCaptchaCallback_Promise = (<any>window).reCaptchaCallback_Promise
  }

  reset(): void {
    this.reCaptchaCallback_Promise.then(grecaptchaObj => grecaptchaObj.reset())
  }

  getOnLoadPromise(): Promise<any> {
    return this.reCaptchaCallback_Promise
  }

  render(el: HTMLElement, params: ReCaptchaParamsInterface): void {
    this.reCaptchaCallback_Promise.then(grecaptchaObj => {
      grecaptchaObj.render(el, params)
    })
  }

  get recapthcaClientKey(): string {
    return '6Lfy0xMUAAAAAFl75Kn67YGjr29FB7GsZ_M1espF' //localStorage.getItem("g_recaptcha");
  }

  get recaptcha_is_disabled(): boolean {
    return false //!!localStorage.getItem("recaptcha_is_disabled");
  }
}

export interface ReCaptchaParamsInterface {
  sitekey: string
  callback(recaptchaToken: string): void
  ['expired-callback'](): void
  ['error-callback'](): void
}
