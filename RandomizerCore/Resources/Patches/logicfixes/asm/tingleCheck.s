.thumb
ldrb	r1,[r0]
mov	r0,#100
sub	r4,r0,r1

@check tingle trophy
ldr	r0,=#0x2002B41
ldrb	r0,[r0]
mov	r1,#0x0C
and	r0,r1
cmp	r0,#0
beq	false

true:
ldrb	r3,=#0x8064A27
bx	r3

false:
ldrb	r3,=#0x8064A41
bx	r3
