.thumb
@check figurine count
ldr	r0,figurines
ldr	r1,=#0x2002AF0
ldrb	r1,[r1]
cmp	r0,r1
bhi	false

true:
mov	r0,#1
bx	lr

false:
mov	r0,#0
bx	lr

.align
.ltorg
figurines:
