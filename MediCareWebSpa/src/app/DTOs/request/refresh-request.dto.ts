export class RefreshRequestDTO {
  refreshToken: string;

  constructor(
    refreshToken: string
  ) {
    this.refreshToken = refreshToken;
  }
}
