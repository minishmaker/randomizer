.thumb
mov	r0,#0
ldr	r1,=#0x2002B42
ldrb	r1,[r1]
mov	r2,#0x55
and	r1,r2
cmp	r1,r2
bne	end
mov	r0,#1
end:
bx	lr
