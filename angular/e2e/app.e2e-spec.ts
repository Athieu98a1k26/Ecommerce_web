import { EcommerceTemplatePage } from './app.po';

describe('Ecommerce App', function() {
  let page: EcommerceTemplatePage;

  beforeEach(() => {
    page = new EcommerceTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
