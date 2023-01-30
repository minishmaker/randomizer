.thumb

ldrb	r0,[r4,#0x0A]
cmp	r0,#0x5C
bne	end
ldrb	r0,[r4,#0x0B]
ldrb	r3,lastAllowedKinstone
cmp	r0,r3
bhi	end
mov	r0,#0x11
strb	r0,[r1]

end:
ldrb	r0,[r4,#0x0E]
mov	r5,r4
add	r5,#0x69
mov	r2,#0
ldrb	r1,=#0x8080A21
bx	r1

.align
.ltorg
lastAllowedKinstone:
