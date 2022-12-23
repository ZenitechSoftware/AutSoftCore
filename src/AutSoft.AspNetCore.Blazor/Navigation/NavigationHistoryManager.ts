class NavigationHistoryManager {
  private static _instance: NavigationHistoryManager;

  private constructor() { }

  public static get Instance() {
    return this._instance || (this._instance = new this());
  }

  public GoBack() {
    history.back();
  }
}
