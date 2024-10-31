// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Load AI-generated reviews
$(document).ready(function() {
    // Check if we're on a details page with reviews section
    if ($('.reviews-section').length) {
        const movieId = window.location.pathname.split('/').pop();
        
        // Load reviews asynchronously
        $.get(`/Movies/GetMovieReviews/${movieId}`)
            .done(function(data) {
                let reviewsHtml = `
                    <div class="sentiment-summary">
                        Overall Sentiment: <span class="sentiment-score">${data.overallSentiment.toFixed(2)}</span>
                    </div>
                    <div class="reviews-container">
                        <table class="table datatables">
                            <thead>
                                <tr>
                                    <th>Review</th>
                                    <th>Sentiment</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${data.reviews.map(r => `
                                    <tr>
                                        <td>${r.review}</td>
                                        <td>${r.sentiment.toFixed(2)}</td>
                                    </tr>
                                `).join('')}
                            </tbody>
                        </table>
                    </div>`;
                
                $('#reviewsContent').html(reviewsHtml);
                // Initialize DataTable
                $('.datatables').DataTable();
            })
            .fail(function() {
                $('#reviewsContent').html('<div class="error-message">Failed to load reviews. Please try again later.</div>');
            });
    }
});

// Load AI-generated tweets
$(document).ready(function() {
    if ($('.social-section').length) {
        const actorId = window.location.pathname.split('/').pop();
        
        $.get(`/Actors/GetActorTweets/${actorId}`)
            .done(function(data) {
                let tweetsHtml = `
                    <div class="sentiment-summary">
                        Overall Sentiment: <span class="sentiment-score">${data.overallSentiment.toFixed(2)}</span>
                    </div>
                    <div class="tweets-container">
                        <table class="table datatables">
                            <thead>
                                <tr>
                                    <th>Social Commentary</th>
                                    <th>Sentiment</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${data.tweets.map(t => `
                                    <tr>
                                        <td>${t.tweet}</td>
                                        <td>${t.sentiment.toFixed(2)}</td>
                                    </tr>
                                `).join('')}
                            </tbody>
                        </table>
                    </div>`;
                
                $('#tweetsContent').html(tweetsHtml);
                $('.datatables').DataTable();
            })
            .fail(function() {
                $('#tweetsContent').html('<div class="error-message">Failed to load social media buzz. Please try again later.</div>');
            });
    }
});

