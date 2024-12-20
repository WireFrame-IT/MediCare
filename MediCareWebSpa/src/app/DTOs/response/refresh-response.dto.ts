export class RefreshResponseDTO {
  accessToken: string;
  refreshToken: string;

  constructor(accessToken: string, refreshToken: string, roleType: number) {
    this.accessToken = accessToken;
    this.refreshToken = refreshToken;
  }
}
