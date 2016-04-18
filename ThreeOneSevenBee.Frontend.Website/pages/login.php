<? require('header.php'); ?>
    <div class="mw300 center">
    <form>
        <input type="text" name="username" placeholder="Brugernavn" />
        <input type="password" name="password" placeholder="Kodeord" />
        <input type="submit" value="login" />
    </form>
        <? if(isAuthed()): ?>
        <a href="/admin">Administrator panel</a>
        <? endif; ?>
    </div>

    <script>
        $('form').on('submit', function(e) {e.preventDefault();});
        $('input[type="submit"]').click(function() {
            $.post("/api/", {
                action: 'user_login',
                username: $('input[name="username"]').val(),
                password: $('input[name="password"]').val()
            },
            function(data) {
                if(JSON.parse(data).success)
                    window.location = '/';
            });
        });
    </script>
<? require('footer.php'); ?>
