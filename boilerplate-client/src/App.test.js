import { render, screen } from '@testing-library/react';
import App from './App';

test('renders login link', () => {
  render(<App />);
  const linkElement = screen.getByRole('link', { name: /login/i });
  expect(linkElement).toBeInTheDocument();
});
