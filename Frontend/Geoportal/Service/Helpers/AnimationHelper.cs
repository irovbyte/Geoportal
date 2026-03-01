namespace Geoportal.Service.Helpers
{
    public static class AnimationHelper
    {
        // ЕДИНЫЕ НАСТРОЙКИ СКОРОСТИ (в миллисекундах)
        private const uint PageInDuration = 250;   // Скорость появления всей страницы
        private const uint CascadeDuration = 400;  // Скорость вылета внутренних элементов
        private const uint ClickDuration = 80;     // Скорость отклика на нажатие

        public static void Prepare(params View[] views)
        {
            foreach (var v in views)
            {
                if (v == null) continue;
                v.Opacity = 0;
                v.Scale = 0.85;
                v.TranslationY = 20;
            }
        }

        public static async Task EntranceAsync(View view, int delayMs = 0)
        {
            if (view == null) return;

            view.Opacity = 0;
            view.Scale = 0.85;
            view.TranslationY = 20;

            if (delayMs > 0) await Task.Delay(delayMs);

            // Используем константу PageInDuration
            await Task.WhenAll(
                view.FadeToAsync(1, PageInDuration, Easing.CubicOut),
                view.ScaleToAsync(1, PageInDuration, Easing.CubicOut),
                view.TranslateToAsync(0, 0, PageInDuration, Easing.CubicOut)
            );
        }

        public static async Task ExecuteFadeInUpAsync(View view, int delayMs = 0)
        {
            if (view == null) return;
            if (delayMs > 0) await Task.Delay(delayMs);

            // Используем константу CascadeDuration
            await Task.WhenAll(
                view.FadeToAsync(1, CascadeDuration, Easing.CubicOut),
                view.TranslateToAsync(0, 0, CascadeDuration, Easing.CubicOut)
            );
        }

        public static async Task ExecuteClickScaleAsync(View view)
        {
            if (view == null) return;
            // Используем константу ClickDuration
            await view.ScaleToAsync(0.92, ClickDuration, Easing.CubicIn);
            await view.ScaleToAsync(1.0, ClickDuration, Easing.CubicOut);
        }
    }
}